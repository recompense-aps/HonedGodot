namespace HonedGodot
{
	public static partial class Constants
	{
		# region Node Signals
		public const string Signal_Node_ChildEnteredTree = "child_entered_tree";
		public const string Signal_Node_ChildExitingTree = "child_exiting_tree";
		public const string Signal_Node_Ready = "ready";
		public const string Signal_Node_Renamed = "renamed";
		public const string Signal_Node_TreeEntered = "tree_entered";
		public const string Signal_Node_TreeExited = "tree_exited";
		public const string Signal_Node_TreeExiting = "tree_exiting";
		#endregion

		public const string Signal_Timer_Timeout = "timeout";

		#region RigidBody2D Signals
		public const string Signal_RigidBody2D_BodyEntered = "body_entered";
		#endregion

		public const string Signal_Area2D_AreaEntered = "area_entered";
		public const string Signal_Area2D_BodyEntered = "body_entered";
		public const string Signal_Area2D_AreaExited = "area_exited";
		public const string Signal_Area2D_BodyExited = "body_exited";
	}
}