package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EnergyUpdateCommand implements IModule
   {
      private static var _instance:EnergyUpdateCommand;
      
      public var maxCaptainEnergy:int = 0;
      
      public var captainEnergyDrain:int = 0;
      
      public var currentCaptainEnergy:int = 0;
      
      public function EnergyUpdateCommand(param1:int = 0, param2:int = 0, param3:int = 0)
      {
         super();
         this.maxCaptainEnergy = param2;
         this.captainEnergyDrain = param3;
         this.currentCaptainEnergy = param1;
      }
      
      public static function get instance() : EnergyUpdateCommand
      {
         return _instance || (_instance = new EnergyUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 30499;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 12;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.maxCaptainEnergy = param1.readInt();
         this.maxCaptainEnergy = this.maxCaptainEnergy << 15 | this.maxCaptainEnergy >>> 17;
         this.captainEnergyDrain = param1.readInt();
         this.captainEnergyDrain = this.captainEnergyDrain >>> 2 | this.captainEnergyDrain << 30;
         this.currentCaptainEnergy = param1.readInt();
         this.currentCaptainEnergy = this.currentCaptainEnergy << 13 | this.currentCaptainEnergy >>> 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(30499);
         param1.writeInt(this.maxCaptainEnergy >>> 15 | this.maxCaptainEnergy << 17);
         param1.writeInt(this.captainEnergyDrain << 2 | this.captainEnergyDrain >>> 30);
         param1.writeInt(this.currentCaptainEnergy >>> 13 | this.currentCaptainEnergy << 19);
      }
   }
}

