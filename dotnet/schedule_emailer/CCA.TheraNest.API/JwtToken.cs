namespace CCA.TheraNest.API
{
    public class JwtToken : IToken
    {
        public string Token { get; set; }

        public override string ToString()
        {
            return Token;
        }
    }
}
