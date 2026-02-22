package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   
   public class TrainingGroundLookingForOpponentStatusCommand extends TrainingGroundStatusDataModule
   {
      private static var _instance:TrainingGroundLookingForOpponentStatusCommand;
      
      public function TrainingGroundLookingForOpponentStatusCommand()
      {
         super();
      }
      
      public static function get instance() : TrainingGroundLookingForOpponentStatusCommand
      {
         return _instance || (_instance = new TrainingGroundLookingForOpponentStatusCommand());
      }
      
      override public function getLibcomModuleId() : int
      {
         return -28076;
      }
      
      override public function getLibcomModuleEstimatedLength() : int
      {
         return 0;
      }
      
      override public function read(param1:IDataInput) : void
      {
         param1.readShort();
         super.read(param1);
      }
      
      override public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-28076);
         super.write(param1);
      }
   }
}

