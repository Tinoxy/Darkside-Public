package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PayloadEscortTimeInfoCommand implements IModule
   {
      private static var _instance:PayloadEscortTimeInfoCommand;
      
      public var timeToNextSpawn:Number = 0;
      
      public var timeToGateClose:Number = 0;
      
      public var timeToSessionEnd:Number = 0;
      
      public function PayloadEscortTimeInfoCommand(param1:Number = 0, param2:Number = 0, param3:Number = 0)
      {
         super();
         this.timeToNextSpawn = param3;
         this.timeToGateClose = param1;
         this.timeToSessionEnd = param2;
      }
      
      public static function get instance() : PayloadEscortTimeInfoCommand
      {
         return _instance || (_instance = new PayloadEscortTimeInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 29135;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 24;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.timeToNextSpawn = param1.readDouble();
         this.timeToGateClose = param1.readDouble();
         this.timeToSessionEnd = param1.readDouble();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(29135);
         param1.writeDouble(this.timeToNextSpawn);
         param1.writeDouble(this.timeToGateClose);
         param1.writeDouble(this.timeToSessionEnd);
      }
   }
}

