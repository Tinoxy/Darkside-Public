package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DisassemblyRequest implements IModule
   {
      private static var _instance:DisassemblyRequest;
      
      public var itemId:Number = 0;
      
      public var recipeLootId:String = "";
      
      public function DisassemblyRequest(param1:String = "", param2:Number = 0)
      {
         super();
         this.itemId = param2;
         this.recipeLootId = param1;
      }
      
      public static function get instance() : DisassemblyRequest
      {
         return _instance || (_instance = new DisassemblyRequest());
      }
      
      public function getLibcomModuleId() : int
      {
         return 19201;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 10;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.itemId = param1.readDouble();
         this.recipeLootId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(19201);
         param1.writeDouble(this.itemId);
         param1.writeUTF(this.recipeLootId);
      }
   }
}

