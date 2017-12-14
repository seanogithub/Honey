using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour {

// Here we store the hash tags for various strings used in our animators.
// Player Bear
    public int speedFloat;
    public int aimWeightFloat;
    public int angularSpeedFloat;

    public int PlayerDeadState;
	public int PlayerDyingState;
    public int PlayerPickUpObjectState;
    public int PlayerDropObjectState;
    public int PlayerCarryObjectState;
    public int PlayerPickUpBaddieState;
    public int PlayerIdleCarryBaddieState;
    public int PlayerWalkCarryBaddieState;
    public int PlayerThrowBaddieState;
/*
    public int deadBool;
	public int dyingState;
    public int pickUpObjectBool;
    public int dropObjectBool;
    public int carryObjectBool;
    public int pickUpBaddieBool;
    public int idleCarryBaddieBool;
    public int walkCarryBaddieBool;
    public int throwBaddieBool;
*/
	
// Baddie Bunny	
    public int IdleState;
    public int RunState;
    public int AttackState;
    public int DieState;
    public int AlertState;
    public int PickUpState;
    public int CarryIdleState;
    public int CarryWalkState;
    public int ThrowState;
    public int FlyState;
    public int LandState;
	
    void Awake ()
    {
// Player Bear		
        speedFloat = Animator.StringToHash("Speed");
        aimWeightFloat = Animator.StringToHash("AimWeight");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");

		PlayerDeadState = Animator.StringToHash("Dead");
        PlayerDyingState = Animator.StringToHash("Base Layer.Dying");
        PlayerPickUpObjectState = Animator.StringToHash("PickUpObject");
        PlayerDropObjectState = Animator.StringToHash("DropObject");
        PlayerCarryObjectState = Animator.StringToHash("CarryObject");		
        PlayerPickUpBaddieState = Animator.StringToHash("PickUpBaddie");		
        PlayerIdleCarryBaddieState = Animator.StringToHash("IdleCarryBaddie");		
        PlayerWalkCarryBaddieState = Animator.StringToHash("WalkCarryBaddie");		
        PlayerThrowBaddieState = Animator.StringToHash("ThrowBaddie");		

		
// Baddie Bunny		
        IdleState = Animator.StringToHash("Idle_State");			
        RunState = Animator.StringToHash("Run_State");			
        AttackState = Animator.StringToHash("Attack_State");			
        DieState = Animator.StringToHash("Die_State");			
        AlertState = Animator.StringToHash("Alert_State");			
		PickUpState = Animator.StringToHash("PickUp_State");			
        CarryIdleState = Animator.StringToHash("CarryIdle_State");			
        CarryWalkState = Animator.StringToHash("CarryWalk_State");			
        ThrowState = Animator.StringToHash("Throw_State");			
        FlyState = Animator.StringToHash("Fly_State");			
        LandState = Animator.StringToHash("Land_State");			
		
    }
}
