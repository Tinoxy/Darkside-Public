package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MenuCategoryInitializationCommand implements IModule
   {
      private static var _instance:MenuCategoryInitializationCommand;
      
      public var categoryId:String = "";
      
      public var menuItems:Vector.<MenuItemInitializationCommand>;
      
      public function MenuCategoryInitializationCommand(param1:String = "", param2:Vector.<MenuItemInitializationCommand> = null)
      {
         super();
         this.categoryId = param1;
         if(param2 == null)
         {
            this.menuItems = new Vector.<MenuItemInitializationCommand>();
         }
         else
         {
            this.menuItems = param2;
         }
      }
      
      public static function get instance() : MenuCategoryInitializationCommand
      {
         return _instance || (_instance = new MenuCategoryInitializationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -28187;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 6;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:MenuItemInitializationCommand = null;
         this.categoryId = param1.readUTF();
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.menuItems.length > 0)
         {
            this.menuItems.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = MenuItemInitializationCommand(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.menuItems.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:MenuItemInitializationCommand = null;
         param1.writeShort(-28187);
         param1.writeUTF(this.categoryId);
         param1.writeByte(this.menuItems.length);
         for each(_loc2_ in this.menuItems)
         {
            _loc2_.write(param1);
         }
      }
   }
}

