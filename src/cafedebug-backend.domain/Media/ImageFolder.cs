namespace cafedebug_backend.domain.Media;

/// <summary>
/// Defines the different folders for organizing files in S3.
/// </summary>
public enum ImageFolder
{
    /// <summary>
    /// Images related to podcast episodes
    /// </summary>
    Episodes,
    
    /// <summary>
    /// Images for banners
    /// </summary>
    Banners,
    
    /// <summary>
    /// Profile pictures of team members (debuggers)
    /// </summary>
    TeamMembers,
    
    /// <summary>
    /// Images for contributors
    /// </summary>
    Contributors
}