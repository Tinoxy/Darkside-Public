package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class InventoryItemRemovedCommand implements IModule
   {
      private static var _instance:InventoryItemRemovedCommand;
      
      public var itemId:Number = 0;
      
      public function InventoryItemRemovedCommand(param1:Number = 0)
      {
         super();
         this.itemId = param1;
      }
      
      public static function get instance() : InventoryItemRemovedCommand
      {
         return _instance || (_instance = new InventoryItemRemovedCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 32109;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.itemId = param1.readDouble();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(32109);
         param1.writeDouble(this.itemId);
      }
   }
}

