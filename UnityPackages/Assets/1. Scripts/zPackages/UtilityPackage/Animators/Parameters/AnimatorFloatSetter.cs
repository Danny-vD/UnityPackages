namespace UtilityPackage.Animators.Parameters
{
	public class AnimatorFloatSetter : AnimatorValueSetter<float>
	{
		public override void SetParameter()
		{
			animator.SetFloat(parameterID, value);
		}
	}
}