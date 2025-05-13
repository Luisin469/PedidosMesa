namespace PedidosMesa.Models
{
    public class LoginRequestModel
    {
        private string _usuario;
        private string _clave;

        public string Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        public string Clave
        {
            get { return _clave; }
            set { _clave = value; }
        }
    }
}
