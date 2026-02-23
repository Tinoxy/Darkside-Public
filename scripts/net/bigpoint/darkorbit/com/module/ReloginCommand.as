package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class ReloginCommand implements IModule
   {
      private static var _instance:ReloginCommand;
      
      public var delayInMillis:int = 0;
      
      public var mapID:int = 0;
      
      public function ReloginCommand(param1:int = 0, param2:int = 0)
      {
         super();
         this.delayInMillis = param2;
         this.mapID = param1;
      }
      
      public static function get instance() : ReloginCommand
      {
         return _instance || (_instance = new ReloginCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -17430;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.delayInMillis = param1.readInt();
         this.delayInMillis = this.delayInMillis >>> 9 | this.delayInMillis << 23;
         this.mapID = param1.readInt();
         this.mapID = this.mapID << 16 | this.mapID >>> 16;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-17430);
         param1.writeInt(this.delayInMillis << 9 | this.delayInMillis >>> 23);
         param1.writeInt(this.mapID >>> 16 | this.mapID << 16);
      }
   }
}

