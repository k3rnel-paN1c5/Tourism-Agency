namespace Domain.Enums;

/// <summary>
/// Defines the possible statuses for a blog post.
/// </summary>
public enum PostStatus
{
    /// <summary>
    /// The post is a draft.
    /// </summary>
    Draft,
    /// <summary>
    /// The post is pending review or publication.
    /// </summary>
    Pending,
    /// <summary>
    /// The post has been published and is visible.
    /// </summary>
    Published,
    /// <summary>
    /// The post is scheduled for future publication.
    /// </summary>
    Scheduled,
    /// <summary>
    /// The post has been unpublished and is no longer visible.
    /// </summary>
    Unpublished,
    /// <summary>
    /// The post has been archived.
    /// </summary>
    Archived,
    /// <summary>
    /// The post has been deleted.
    /// </summary>
    Deleted
}
