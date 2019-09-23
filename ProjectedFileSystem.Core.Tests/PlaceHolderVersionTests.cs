using ProjectedFileSystem.Core.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjectedFileSystem.Core.Tests
{
  public class PlaceholderVersionTests
  {
    [Fact]
    public void Empty_Versions_Compare_Equal()
    {
      //Arrange
      var version1 = new PlaceholderVersion(new byte[0], new byte[0]);
      var version2 = new PlaceholderVersion(new byte[0], new byte[0]);

      //Act
      var result = version1 == version2;

      //Assert
      Assert.True(result);
    }

    [Fact]
    public void No_Oversized_ProviderID()
    {
      //Arrange
      var provisionalProviderID = new byte[129];
      var provisionalContentID = new byte[0];

      //Assert
      Assert.Throws<ArgumentOutOfRangeException>(() => new PlaceholderVersion(provisionalProviderID, provisionalContentID));
    }

    [Fact]
    public void No_Oversized_ContentID()
    {
      //Arrange
      var provisionalProviderID = new byte[0];
      var provisionalContentID = new byte[129];

      //Assert
      Assert.Throws<ArgumentOutOfRangeException>(() => new PlaceholderVersion(provisionalProviderID, provisionalContentID));
    }

    [Fact]
    public void Faithful_Return_ProviderID()
    {
      //Arrange
      var providerID = new byte[] { 1, 19, 3 };
      var contentID = new byte[0];
      var target = new PlaceholderVersion(providerID, contentID);

      //Act
      var newProviderID = target.ProviderId;

      //Assert
      Assert.Equal(3, newProviderID.Length);
      Assert.Equal(1, newProviderID[0]);
      Assert.Equal(19, newProviderID[1]);
      Assert.Equal(3, newProviderID[2]);
    }

    [Fact]
    public void Faithful_Return_ContentID()
    {
      //Arrange
      var providerID = new byte[0];
      var contentID = new byte[] { 37, 91 };
      var target = new PlaceholderVersion(providerID, contentID);

      //Act
      var newContentID = target.ContentId;

      //Assert
      Assert.Equal(2, newContentID.Length);
      Assert.Equal(37, newContentID[0]);
      Assert.Equal(91, newContentID[1]);
    }

    [Fact]
    public void Multibyte_Representation_Equality()
    {
      //Arrange
      var version1 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7 });
      var version2 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7 });

      //Act
      var result = version1 == version2;

      //Assert
      Assert.True(result);
    }
    [Fact]
    public void Trailing_Zeros_Dont_Change_Equality()
    {
      //Arrange
      var version1 = new PlaceholderVersion(new byte[] { 1, 2, 3, 0, 0, 0 }, new byte[] { 4, 5, 6, 7 });
      var version2 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7, 0 });

      //Act
      var result = version1 == version2;

      //Assert
      Assert.True(result);
    }

    [Fact]
    public void Equal_Via_Object_Equals()
    {
      //Arrange
      var version1 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7 });
      var version2 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7 });

      //Act
      var result = version1.Equals((object)version2);

      //Assert
      Assert.True(result);
    }
    [Fact]
    public void Trailing_NonZeros_Do_Change_Equality()
    {
      //Arrange
      var version1 = new PlaceholderVersion(new byte[] { 1, 2, 3, 0, 0, 1 }, new byte[] { 4, 5, 6, 7 });
      var version2 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7, 0 });

      //Act
      var result = version1 == version2;

      //Assert
      Assert.False(result);
    }
    [Fact]
    public void Non_Null_Not_Equal_To_Null_Right()
    {
      //Arrange
      var version = new PlaceholderVersion(new byte[0], new byte[0]);

      //Act
      var result = version == null;

      //Assert
      Assert.False(result);
    }
    [Fact]
    public void Non_Null_Not_Equal_To_Null_Left()
    {
      //Arrange
      var version = new PlaceholderVersion(new byte[0], new byte[0]);

      //Act
      var result = null == version;

      //Assert
      Assert.False(result);
    }
    [Fact]
    public void Non_Null_NotEqual_To_Null_Right()
    {
      //Arrange
      var version = new PlaceholderVersion(new byte[0], new byte[0]);

      //Act
      var result = version != null;

      //Assert
      Assert.True(result);
    }
    [Fact]
    public void Non_Null_NotEqual_To_Null_Left()
    {
      //Arrange
      var version = new PlaceholderVersion(new byte[0], new byte[0]);

      //Act
      var result = null != version;

      //Assert
      Assert.True(result);
    }

    [Fact]
    public void Hashes_Are_Equal()
    {
      //Arrange
      var version1 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7 });
      var version2 = new PlaceholderVersion(new byte[] { 1, 2, 3 }, new byte[] { 4, 5, 6, 7 });

      //Act
      var result = version1.GetHashCode() == version2.GetHashCode();

      //Assert
      Assert.True(result);
    }
  }
}
