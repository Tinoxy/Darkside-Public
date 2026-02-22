package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class TitleFrenzyOwnerUpdateCommand implements IModule
   {
      private static var _instance:TitleFrenzyOwnerUpdateCommand;
      
      public var username:String = "";
      
      public var mapId:int = 0;
      
      public var titleId:int = 0;
      
      public function TitleFrenzyOwnerUpdateCommand(param1:String = "", param2:int = 0, param3:int = 0)
      {
         super();
         this.username = param1;
         this.mapId = param2;
         this.titleId = param3;
      }
      
      public static function get instance() : TitleFrenzyOwnerUpdateCommand
      {
         return _instance || (_instance = new TitleFrenzyOwnerUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -25017;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.username = param1.readUTF();
         this.mapId = param1.readInt();
         this.mapId = this.mapId << 8 | this.mapId >>> 24;
         this.titleId = param1.readInt();
         this.titleId = this.titleId << 13 | this.titleId >>> 19;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-25017);
         param1.writeUTF(this.username);
         param1.writeInt(this.mapId >>> 8 | this.mapId << 24);
         param1.writeInt(this.titleId >>> 13 | this.titleId << 19);
      }
   }
}

