package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class JumpCPUSelectCommand implements IModule
   {
      private static var _instance:JumpCPUSelectCommand;
      
      public var mapId:int = 0;
      
      public var price:int = 0;
      
      public var seletionAllowed:Boolean = false;
      
      public function JumpCPUSelectCommand(param1:int = 0, param2:int = 0, param3:Boolean = false)
      {
         super();
         this.mapId = param1;
         this.price = param2;
         this.seletionAllowed = param3;
      }
      
      public static function get instance() : JumpCPUSelectCommand
      {
         return _instance || (_instance = new JumpCPUSelectCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 63;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 9;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.mapId = param1.readInt();
         this.mapId = this.mapId << 13 | this.mapId >>> 19;
         this.price = param1.readInt();
         this.price = this.price >>> 9 | this.price << 23;
         this.seletionAllowed = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(63);
         param1.writeInt(this.mapId >>> 13 | this.mapId << 19);
         param1.writeInt(this.price << 9 | this.price >>> 23);
         param1.writeBoolean(this.seletionAllowed);
      }
   }
}

