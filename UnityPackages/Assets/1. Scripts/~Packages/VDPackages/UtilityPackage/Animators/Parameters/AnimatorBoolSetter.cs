namespace VDPackages.UtilityPackage.Animators.Parameters
{
	public class AnimatorBoolSetter : AnimatorValueSetter<bool>
	{
		public override void SetParameter()
		{
			animator.SetBool(parameterID, value);
		}
	}
}