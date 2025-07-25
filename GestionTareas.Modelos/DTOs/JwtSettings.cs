namespace GestionTareas.Modelos.DTOs
{
    public class JwtSettings
    {
        public string Key { get; set; } // Clave secreta para firmar el token JWT

        public string Issuer { get; set; } // Emisor del token JWT, generalmente el nombre de la aplicación o servicio

        public string Audience { get; set; } // Audiencia del token JWT, generalmente el nombre de la aplicación o servicio que consumirá el token

        public int ExpireMinutes { get; set; } // Tiempo de expiración del token JWT en minutos
    }
}
