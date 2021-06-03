/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: IValidation
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 

/// <summary>
/// Application definition
/// </summary>
namespace RemoveExpiredGuests.Bases.Definitions
{
    /// <summary>
    /// The require validation
    /// </summary>
    public interface IValidation
    {
        /// <summary>
        /// Validate or initialize default object value
        /// </summary>
        void Validate();
    }
}