package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class PVPSummerEventFeverInfoCommand implements IModule
   {
      private static var _instance:PVPSummerEventFeverInfoCommand;
      
      public var timeToNextFever:Number = 0;
      
      public var timeToEndFever:Number = 0;
      
      public function PVPSummerEventFeverInfoCommand(param1:Number = 0, param2:Number = 0)
      {
         super();
         this.timeToNextFever = param2;
         this.timeToEndFever = param1;
      }
      
      public static function get instance() : PVPSummerEventFeverInfoCommand
      {
         return _instance || (_instance = new PVPSummerEventFeverInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -23311;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 16;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.timeToNextFever = param1.readDouble();
         this.timeToEndFever = param1.readDouble();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-23311);
         param1.writeDouble(this.timeToNextFever);
         param1.writeDouble(this.timeToEndFever);
      }
   }
}

