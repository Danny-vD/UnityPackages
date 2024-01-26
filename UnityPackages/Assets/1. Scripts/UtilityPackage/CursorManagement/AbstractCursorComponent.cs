using UtilityPackage.CursorManagement.Structs;
using VDFramework;

namespace UtilityPackage.CursorManagement
{
	/// <summary>
	/// An abstract representation of a component used by the <seealso cref="CursorComponentManager"/> to change cursor textures and add cursor effects
	/// </summary>
	public abstract class AbstractCursorComponent : BetterMonoBehaviour
	{
		/// <summary>
		/// Is this component current activated?
		/// </summary>
		/// <seealso cref="Activate"/>
		/// <seealso cref="Deactivate"/>
		public bool IsActive { get; private set; }
		
		/// <summary>
		/// If true, another component (that is lower in the priority list) may be activate alongside this one
		/// </summary>
		/// <seealso cref="CursorComponentManager.cursorComponents"/>
		public abstract bool IsAdditiveEffect { get; }

		/// <summary>
		/// <para>Whether the <see cref="CursorComponentManager"/> should call <see cref="GetCursorData"/> to update the cursor texture</para>
		/// </summary>
		public bool ShouldUpdateCursor { get; protected set; }
		
		/// <summary>
		/// <para>If true, then this component is ready to be activated</para>
		/// <para>This is called every <see cref="CursorComponentManager.LateUpdate"/> in the <see cref="CursorComponentManager"/></para>
		/// </summary>
		public abstract bool AreConditionsMet();

		/// <summary>
		/// Will be called the first time the conditions are met
		/// </summary>
		/// <seealso cref="AreConditionsMet"/>
		public void Activate()
		{
			IsActive = true;
			OnActivate();
		}

		/// <summary>
		/// Will be called the first time the conditions are not met
		/// </summary>
        /// <seealso cref="AreConditionsMet"/>
		public void Deactivate()
		{
			IsActive = false;
			
			OnDeactivate();
		}

		/// <summary>
		/// <para>Returns the texture that the cursor should update to, including hotspot</para>
		/// <para>Only called if <see cref="ShouldUpdateCursor"/> is true</para>
		/// </summary>
		public abstract CursorData GetCursorData();
		
		protected virtual void OnActivate()
		{
		}
		
		protected virtual void OnDeactivate()
		{
		}
	}
}