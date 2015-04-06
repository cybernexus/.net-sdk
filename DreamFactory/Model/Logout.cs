﻿// ReSharper disable InconsistentNaming
namespace DreamFactory.Model
{
    /// <summary>
    /// Logout response.
    /// </summary>
    public class Logout : IModel
    {
        /// <summary>
        /// Gets flag indicating logout request succeeded or not.
        /// </summary>
        public bool success { get; set; }
    }
}
