using System.Runtime.CompilerServices;
using VixenModules.Property.Color;
using Xunit;

namespace Vixen.Tests.Property.ColorPropertyModule;

public class ColorModuleTests
{
	[Fact]
	public void Colors_AfterColorSetChanged_ReturnsUpdatedColorSet()
	{
		// Arrange
		var staticData = new ColorStaticData();
		var module = CreateModule(staticData, "RGB");
		Assert.Equal(3, module.Colors.Count);
		var replacement = new ColorSet { System.Drawing.Color.White };

		// Act
		staticData.SetColorSet("RGB", replacement);

		// Assert
		Assert.Same(replacement, module.Colors);
	}

	[Fact]
	public void ColorModule_WhenNoLongerReferenced_CanBeCollected()
	{
		// Arrange
		var staticData = new ColorStaticData();
		var moduleReference = CreateUnreferencedModule(staticData);

		// Act
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		// Assert
		Assert.False(moduleReference.IsAlive);
	}

	private static ColorModule CreateModule(ColorStaticData staticData, string colorSetName)
	{
		var module = new ColorModule
		{
			StaticModuleData = staticData,
			ModuleData = new ColorData()
		};
		module.ColorType = ElementColorType.MultipleDiscreteColors;
		module.ColorSetName = colorSetName;
		return module;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static WeakReference CreateUnreferencedModule(ColorStaticData staticData)
	{
		var module = CreateModule(staticData, "RGB");
		_ = module.Colors;
		return new WeakReference(module);
	}
}
