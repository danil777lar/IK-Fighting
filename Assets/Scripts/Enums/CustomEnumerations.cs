namespace LarjeEnum 
{
    public enum KinematicsPointerType { Body, FrontArm, BackArm, FrontLeg, BackLeg }

    public enum PointerMotion 
    {
        None,
        BodyCalm,
        ArmCalm,
        ArmPunch,
        LegCalm,
        LegStep,
        LegToNormalDistance
    }
}
