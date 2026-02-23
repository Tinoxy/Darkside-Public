package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class EmoteActivationCommand implements IModule
   {
      private static var _instance:EmoteActivationCommand;
      
      public var lootId:String = "";
      
      public var userId:int = 0;
      
      public function EmoteActivationCommand(param1:String = "", param2:int = 0)
      {
         super();
         this.lootId = param1;
         this.userId = param2;
      }
      
      public static function get instance() : EmoteActivationCommand
      {
         return _instance || (_instance = new EmoteActivationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 21929;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.lootId = param1.readUTF();
         this.userId = param1.readInt();
         this.userId = this.userId << 5 | this.userId >>> 27;
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(21929);
         param1.writeUTF(this.lootId);
         param1.writeInt(this.userId >>> 5 | this.userId << 27);
      }
   }
}

