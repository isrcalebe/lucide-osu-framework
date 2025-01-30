namespace Lucide.OsuFramework.Tests.Visual;

public abstract partial class LucideTestScene : TestScene
{
    protected override ITestSceneTestRunner CreateRunner()
        => new LucideTestRunner();

    private partial class LucideTestRunner : LucideTestGame, ITestSceneTestRunner
    {
        private TestSceneTestRunner.TestRunner? runner;

        protected override void LoadAsyncComplete()
        {
            base.LoadAsyncComplete();

            Add(runner = new TestSceneTestRunner.TestRunner());
        }

        public void RunTestBlocking(TestScene test)
            => runner?.RunTestBlocking(test);
    }
}
