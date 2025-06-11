using System.Collections.Concurrent;

namespace EncuestasAPI.Helpers
{
	public class UsuariosActivosStore
	{
		private static readonly Dictionary<string, string> _usuarios = new();

		public static void AgregarUsuario(string connectionId, string nombre)
		{
			lock (_usuarios)
			{
				_usuarios[connectionId] = nombre;
			}
		}

		public static void RemoverUsuario(string connectionId)
		{
			lock (_usuarios)
			{
				if (_usuarios.ContainsKey(connectionId))
					_usuarios.Remove(connectionId);
			}
		}

		public static List<string> ObtenerUsuariosActivos()
		{
			lock (_usuarios)
			{
				return _usuarios.Values.Distinct().ToList();
			}
		}
	}
}
