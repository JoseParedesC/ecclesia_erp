namespace Ecclesia.Api.Constants;

/// <summary>
/// Constantes de permisos para políticas de autorización.
/// Agregar dentro de la clase EcclesiaPermissions existente si ya existe,
/// o usar este archivo como base.
/// </summary>
public static class EcclesiaPermissions
{
    public static class Rostros
    {
        private const string Base = "org.rostro";

        /// <summary>Permite listar y consultar Rostros.</summary>
        public const string Read       = $"{Base}.read";

        /// <summary>Permite crear un nuevo Rostro.</summary>
        public const string Create     = $"{Base}.create";

        /// <summary>Permite actualizar nombre y descripción de un Rostro activo.</summary>
        public const string Update     = $"{Base}.update";

        /// <summary>Permite desactivar un Rostro (eliminación lógica).</summary>
        public const string Deactivate = $"{Base}.deactivate";
    }
}
