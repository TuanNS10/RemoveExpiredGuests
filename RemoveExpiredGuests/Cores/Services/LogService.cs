/// ====================================================================================================
/// System name: Remove expired guest on Azure Active Directory
///  Class name: LogService
///      Verion: 1.0.0.0
///
/// Change History
/// ----------------------------------------------------------------------------------------------------
/// Date            Author          Content
/// ----------------------------------------------------------------------------------------------------
/// 2021.05.12      SanhTuan        Create
/// ====================================================================================================
/// 
using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using RemoveExpiredGuests.Bases.Definitions;

/// <summary>
/// Application services
/// </summary>
namespace RemoveExpiredGuests.Cores.Services
{
    /// Instant using methods
    using static RemoveExpiredGuests.Bases.Definitions.Constants;

    /// <summary>
    /// Application log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Error log level
        /// </summary>
        Error,

        /// <summary>
        /// Warning log level
        /// </summary>
        Warning,

        /// <summary>
        /// Success log level
        /// </summary>
        Success,
    }

    
    
}