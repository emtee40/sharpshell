using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ResourcesPropertySheet.Loader;

namespace ResourcesPropertySheet.Tests
{
    /// <summary>
    /// These tests are functional tests rather than unit tests, but they do the trick and provide
    /// a pretty reasonable degree of confidence that this is all working.
    /// </summary>
    [TestFixture]
    public class ResourceLoaderTests
    {
        [Test]
        public void CanLoadResourceTypes()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"TestFiles\DllWithResources.dll");
            var resources = ResourceLoader.LoadResources(path);
            var resourceTypeStrings = resources.Select(rt => rt.ResourceType.ToString()).ToArray();

            //  Assert we have the expected set of resource types.
            Assert.That(resourceTypeStrings.Contains("Bitmap"));
            Assert.That(resourceTypeStrings.Contains("Cursor"));
            Assert.That(resourceTypeStrings.Contains("Dialog"));
            Assert.That(resourceTypeStrings.Contains("HTML"));
            Assert.That(resourceTypeStrings.Contains("Group Cursor"));
            Assert.That(resourceTypeStrings.Contains("Group Icon"));
            Assert.That(resourceTypeStrings.Contains("Icon"));
            Assert.That(resourceTypeStrings.Contains("Menu"));
            Assert.That(resourceTypeStrings.Contains("\"PNG\""));
            Assert.That(resourceTypeStrings.Contains("RT_MANIFEST"));
            Assert.That(resourceTypeStrings.Contains("\"RT_RIBBON_XML\""));
            Assert.That(resourceTypeStrings.Contains("241")); // toolbars
            Assert.That(resourceTypeStrings.Contains("Version"));

            //  Check we have loaded a bitmap property.
            var bitmaps = resources.Single(rt => rt.ResourceType.IsKnownResourceType(ResType.RT_BITMAP));
            var bitmap103 = bitmaps.Resouces.Single(b => b.ResourceName.IsInt && b.ResourceName.IntValue == 103);
            Assert.That(bitmap103.ResourceName.IsInt);
            Assert.That(bitmap103.ResourceName.IntValue, Is.EqualTo(103));
            Assert.That(bitmap103.ResourceName.ToString(), Is.EqualTo("103"));
            Assert.That(bitmap103.BitmapData.Width, Is.EqualTo(48));
            Assert.That(bitmap103.BitmapData.Height, Is.EqualTo(48));
        }
    }
}
