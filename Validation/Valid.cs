namespace hwSDI.Validation
{
    public class Valid
    {
        public Valid() { }

        public bool ValidYear(int year)
        {
            return year > 1500 && year <= DateTime.Now.Year;
        }

        public bool ValidText(string text)
        {
            return text.Length != 0;
        }

        public bool ValidEmail(string email)
        {
            int at = 0;
            int dot = 0;
            foreach (char c in email)
            {
                if (c == '@')
                {
                    at++;
                }
                else
                    if (c == '.')
                {
                    dot++;
                }
                else
                    if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')))
                        return false;
            }
            if (at != 1)
                return false;
            if (dot != 1)
                return false;
            return true;
        }
    }
}
