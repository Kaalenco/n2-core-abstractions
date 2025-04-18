namespace N2.Core.Dms;

/// <summary>
/// Refer to dublin core metadata document type definitions,
/// describing the nature or genre of the resource.
/// https://www.dublincore.org/specifications/dublin-core/dcmi-type-vocabulary/2000-07-11/
/// </summary>
public enum AttachmentType
{
    Unspecified = 0,
    Collection = 1,
    Dataset = 2,
    Event = 3,
    Image = 4,
    InteractiveResource = 5,
    Service = 6,
    Software = 7,
    Sound = 8,
    Text = 9,
}
