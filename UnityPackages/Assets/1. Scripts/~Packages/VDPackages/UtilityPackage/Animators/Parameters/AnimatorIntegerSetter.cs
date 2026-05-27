namespace VDPackages.UtilityPackage.Animators.Parameters
{
	public class AnimatorIntegerSetter : AnimatorValueSetter<int>
	{
		public override void SetParameter()
		{
			animator.SetInteger(parameterID, value);
		}
	}
}