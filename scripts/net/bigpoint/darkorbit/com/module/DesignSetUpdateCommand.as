package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DesignSetUpdateCommand implements IModule
   {
      private static var _instance:DesignSetUpdateCommand;
      
      public var name:String = "";
      
      public var active:Boolean = false;
      
      public var userId:int = 0;
      
      public function DesignSetUpdateCommand(param1:int = 0, param2:String = "", param3:Boolean = false)
      {
         super();
         this.name = param2;
         this.active = param3;
         this.userId = param1;
      }
      
      public static function get instance() : DesignSetUpdateCommand
      {
         return _instance || (_instance = new DesignSetUpdateCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 19846;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 7;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.name = param1.readUTF();
         this.active = param1.readBoolean();
         this.userId = param1.readInt();
         this.userId = this.userId << 6 | this.userId >>> 26;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(19846);
         param1.writeUTF(this.name);
         param1.writeBoolean(this.active);
         param1.writeInt(this.userId >>> 6 | this.userId << 26);
      }
   }
}

