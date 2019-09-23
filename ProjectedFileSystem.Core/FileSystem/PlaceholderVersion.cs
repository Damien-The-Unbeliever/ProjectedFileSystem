using ProjectedFileSystem.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectedFileSystem.Core.FileSystem
{
  /// <summary>
  /// Which version of a file/directory a particular placeholder represents
  /// </summary>
  public class PlaceholderVersion : IEquatable<PlaceholderVersion>
  {
    private readonly ReadOnlyMemory<byte> _providerId;
    private readonly ReadOnlyMemory<byte> _contentId;
    /// <summary>
    /// Construct a new version instance
    /// </summary>
    /// <param name="providerId">This should be up to 128 bytes that identify the provider. These may contain versioning information about the <paramref name="contentId"/></param>
    /// <param name="contentId">This should be up to 128 bytes that identify a specific version of a file</param>
    public PlaceholderVersion(ReadOnlyMemory<byte> providerId, ReadOnlyMemory<byte> contentId)
    {
      if (providerId.Length > Constants.PRJ_PLACEHOLDER_ID_LENGTH) throw new ArgumentOutOfRangeException(nameof(providerId));
      if (contentId.Length > Constants.PRJ_PLACEHOLDER_ID_LENGTH) throw new ArgumentOutOfRangeException(nameof(contentId));
       _providerId = providerId;
      _contentId = contentId;
    }

    /// <summary>
    /// Up to 128 bytes representing the provider
    /// </summary>
    public ReadOnlySpan<byte> ProviderId => _providerId.Span;
    /// <summary>
    /// Up to 128 bytes representing the specific version of a file
    /// </summary>
    public ReadOnlySpan<byte> ContentId => _contentId.Span;

    #region Equality
    public bool Equals(PlaceholderVersion other)
    {
      if (other is null) return false;
      return EqualBuffers(_providerId, other._providerId) && EqualBuffers(_contentId, other._contentId);
    }

    private static bool EqualBuffers(ReadOnlyMemory<byte> left, ReadOnlyMemory<byte> right)
    {
      var commonLength = Math.Min(left.Length, right.Length);
      var lspan = left.Span;
      var rspan = right.Span;
      for(int i = 0; i < commonLength; i++)
      {
        if (lspan[i] != rspan[i]) return false;
      }
      for(int j = commonLength; j < left.Length; j++)
      {
        if (lspan[j] != 0) return false;
      }
      for(int k = commonLength; k< right.Length; k++)
      {
        if (rspan[k] != 0) return false;
      }
      return true;
    }

    public static bool operator==(PlaceholderVersion left, PlaceholderVersion right)
    {
      if (!(left is null)) return left.Equals(right);
      return right is null;
    }
    public static bool operator !=(PlaceholderVersion left, PlaceholderVersion right)
    {
      if (!(left is null)) return !left.Equals(right);
      return !(right is null);
    }
    public override bool Equals(object obj)
    {
      return Equals(obj as PlaceholderVersion);
    }
    public override int GetHashCode()
    {
      int result = 17;
      unchecked
      {
        foreach(var b in this._providerId.Span)
        {
          if (b != 0)
          {
            result = ((result << 7) | (result >> 32 - 7)) ^ b;
          }
        }
        foreach(var b in this._contentId.Span)
        {
          if (b != 0)
          {
            result = ((result << 8) | (result >> 32 - 8)) ^ 0xFE00 ^ b;
          }
        }
      }
      return result;
    }
    #endregion
  }
}
