namespace VDPackages.UtilityPackage.Animators.Parameters
{
	public class AnimatorTriggerSetter : AnimatorParameterSetter
	{
		public override void SetParameter()
		{
			animator.SetTrigger(parameterID);
		}
	}
}