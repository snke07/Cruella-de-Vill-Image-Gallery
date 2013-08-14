namespace CruellaDeVillImageGallery.Repositories
{
    using CruellaDeVillImageGallery.Models;
    using CruellaDeVillImageGallery.Data;
    using System.Linq;
    using System.Text;
    using System.Net.Mail;

    public class UsersRepository : BaseRepository
    {
        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;

        private const string ValidEmailChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_1234567890@.";
        private const string ValidNicknameChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM_1234567890-";
        private const int MinEmailChars = 8;
        private const int MaxEmailChars = 30;
        private const int MinNicknameChars = 4;
        private const int MaxNicknameChars = 16;

        private static void ValidateSessionKey(string sessionKey)
        {
            if (sessionKey.Length != SessionKeyLen || sessionKey.Any(ch => !SessionKeyChars.Contains(ch)))
            {
                throw new ServerErrorException("Invalid Password", "ERR_INV_AUTH");
            }
        }

        private static string GenerateSessionKey(int userId)
        {
            StringBuilder keyChars = new StringBuilder(50);
            keyChars.Append(userId.ToString());
            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (rand)
                {
                    randomCharNum = rand.Next(SessionKeyChars.Length);
                }
                char randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }
            string sessionKey = keyChars.ToString();
            return sessionKey;
        }

        private static void ValidateEmail(string email)
        {
            try
            {
                var m = new MailAddress(email);
            }
            catch
            {
                if (email == null || email.Length < MinEmailChars || email.Length > MaxEmailChars)
                {
                    throw new ServerErrorException("Email should be between 8 and 30 symbols long", "INV_EMAIL_LEN");
                }
                else if (email.Any(ch => !ValidEmailChars.Contains(ch)))
                {
                    throw new ServerErrorException("Email contains invalid characters", "INV_EMAIL_CHARS");
                }
            }
        }

        private static void ValidateNickname(string nickname)
        {
            if (nickname == null || nickname.Length < MinNicknameChars || nickname.Length > MaxNicknameChars)
            {
                throw new ServerErrorException("Nickname should be between 4 and 30 symbols long", "INV_NICK_LEN");
            }
            else if (nickname.Any(ch => !ValidNicknameChars.Contains(ch)))
            {
                throw new ServerErrorException("Nickname contains invalid characters", "INV_NICK_CHARS");
            }
        }

        private static void ValidateAuthCode(string authCode)
        {
            if (authCode.Length != Sha1CodeLength)
            {
                throw new ServerErrorException("Invalid authentication code length", "INV_EMAIL_AUTH_LEN");
            }
        }

        public static void CreateUser(string email, string nickname, string authCode)
        {
            ValidateEmail(email);
            ValidateNickname(nickname);
            ValidateAuthCode(authCode);

            using (var context = new ImageLibraryEntities())
            {
                var emailToLower = email.ToLower();
                var nicknameToLower = nickname.ToLower();

                var dbUser = context.Users
                    .FirstOrDefault(u => u.Email.ToLower() == emailToLower || u.Nickname.ToLower() == nicknameToLower);

                if (dbUser != null)
                {
                    if (dbUser.Email.ToLower() == emailToLower)
                    {
                        throw new ServerErrorException("Email already exists", "ERR_DUP_EMAIL");
                    }
                    else
                    {
                        throw new ServerErrorException("Nickname already exists", "ERR_DUP_NICK");
                    }
                }

                dbUser = new User()
                {
                    Email = emailToLower,
                    Nickname = nickname,
                    AuthCode = authCode
                };

                context.Users.Add(dbUser);
                context.SaveChanges();
            }
        }

        public static string LoginUser(string email, string authCode, out string nickname)
        {
            ValidateEmail(email);
            ValidateAuthCode(authCode);

            using (var context = new ImageLibraryEntities())
            {
                var emailToLower = email.ToLower();

                var user = context.Users
                    .FirstOrDefault(u => u.Email.ToLower() == emailToLower && u.AuthCode == authCode);

                if (user == null)
                {
                    throw new ServerErrorException("Invalid user authentication", "INV_USR_AUTH");
                }

                var sessionKey = GenerateSessionKey((int)user.Id);
                user.SessionKey = sessionKey;
                nickname = user.Nickname;
                context.SaveChanges();

                return sessionKey;
            }
        }

        public static int LoginUser(string sessionKey)
        {
            ValidateSessionKey(sessionKey);

            using (var context = new ImageLibraryEntities())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.SessionKey == sessionKey);

                if (user == null)
                {
                    throw new ServerErrorException("Invalid user authentication", "INV_USR_AUTH");
                }

                return (int)user.Id;
            }
        }

        public static void LogoutUser(string sessionKey)
        {
            ValidateSessionKey(sessionKey);

            using (var context = new ImageLibraryEntities())
            {
                var user = context.Users
                    .FirstOrDefault(u => u.SessionKey == sessionKey);

                if (user == null)
                {
                    throw new ServerErrorException("Invalid user authentication", "INV_USR_AUTH");
                }

                user.SessionKey = null;
                context.SaveChanges();
            }
        }
    }
}