package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class SpacePlagueInfectionStatusCommand implements IModule
   {
      private static var _instance:SpacePlagueInfectionStatusCommand;
      
      public var remainingTime:int = 0;
      
      public var infected:Boolean = false;
      
      public function SpacePlagueInfectionStatusCommand(param1:Boolean = false, param2:int = 0)
      {
         super();
         this.remainingTime = param2;
         this.infected = param1;
      }
      
      public static function get instance() : SpacePlagueInfectionStatusCommand
      {
         return _instance || (_instance = new SpacePlagueInfectionStatusCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 25687;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 5;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.remainingTime = param1.readInt();
         this.remainingTime = this.remainingTime >>> 16 | this.remainingTime << 16;
         this.infected = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(25687);
         param1.writeInt(this.remainingTime << 16 | this.remainingTime >>> 16);
         param1.writeBoolean(this.infected);
      }
   }
}

