public class RangedUnit : MyUnit
{
	public override void Attack(MyUnit target)
	{
		base.Attack(target); // Ensure the animation state is also changed to Attack here
	}
}