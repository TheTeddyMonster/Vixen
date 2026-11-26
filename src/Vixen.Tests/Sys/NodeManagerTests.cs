using System.Runtime.CompilerServices;
using Vixen.Sys.Managers;
using Xunit;

namespace Vixen.Tests.Sys;

public class NodeManagerTests
{
	[Fact]
	public void Dispose_AllowsReplacedManagerToBeCollected()
	{
		// Arrange
		var managerReference = CreateDisposedManager();

		// Act
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();

		// Assert
		Assert.False(managerReference.IsAlive);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static WeakReference CreateDisposedManager()
	{
		var manager = new NodeManager();
		manager.Dispose();
		return new WeakReference(manager);
	}
}
