package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class NameChangeCommand implements IModule
   {
      private static var _instance:NameChangeCommand;
      
      public var newName:String = "";
      
      public var userId:int = 0;
      
      public function NameChangeCommand(param1:int = 0, param2:String = "")
      {
         super();
         this.newName = param2;
         this.userId = param1;
      }
      
      public static function get instance() : NameChangeCommand
      {
         return _instance || (_instance = new NameChangeCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -29266;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.newName = param1.readUTF();
         this.userId = param1.readInt();
         this.userId = this.userId >>> 2 | this.userId << 30;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-29266);
         param1.writeUTF(this.newName);
         param1.writeInt(this.userId << 2 | this.userId >>> 30);
      }
   }
}

