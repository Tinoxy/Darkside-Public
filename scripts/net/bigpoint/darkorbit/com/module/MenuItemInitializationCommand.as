package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MenuItemInitializationCommand implements IModule
   {
      private static var _instance:MenuItemInitializationCommand;
      
      public static const ACTIONSTYLE_ONE_SHOT:int = 0;
      
      public static const ACTIONSTYLE_TOGGLE:int = 1;
      
      public static const ACTIONSTYLE_SELECTION:int = 2;
      
      public static const ACTIONSTYLE_TIMER:int = 3;
      
      public static const ACTIONSTYLE__MAX:int = 4;
      
      public static const COUNTERTYPE_NONE:int = 0;
      
      public static const COUNTERTYPE_NUMBER:int = 1;
      
      public static const COUNTERTYPE_BAR:int = 2;
      
      public static const COUNTERTYPE_DOTS:int = 3;
      
      public static const COUNTERTYPE__MAX:int = 4;
      
      public var counterType:int = 0;
      
      public var defaultIndex:int = 0;
      
      public var timer:MenuItemTimerCommand;
      
      public var actionStyle:int = 0;
      
      public var showTooltipCooldownTimer:Boolean = false;
      
      public var status:MenuItemStatusCommand;
      
      public var cooldownGroup:CooldownTypeModule;
      
      public function MenuItemInitializationCommand(param1:int = 0, param2:MenuItemStatusCommand = null, param3:MenuItemTimerCommand = null, param4:CooldownTypeModule = null, param5:int = 0, param6:int = 0, param7:Boolean = false)
      {
         super();
         this.counterType = param5;
         this.defaultIndex = param1;
         if(param3 == null)
         {
            this.timer = new MenuItemTimerCommand();
         }
         else
         {
            this.timer = param3;
         }
         this.actionStyle = param6;
         this.showTooltipCooldownTimer = param7;
         if(param2 == null)
         {
            this.status = new MenuItemStatusCommand();
         }
         else
         {
            this.status = param2;
         }
         if(param4 == null)
         {
            this.cooldownGroup = new CooldownTypeModule();
         }
         else
         {
            this.cooldownGroup = param4;
         }
      }
      
      public static function get instance() : MenuItemInitializationCommand
      {
         return _instance || (_instance = new MenuItemInitializationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 9903;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 5;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.counterType = param1.readShort();
         this.defaultIndex = param1.readInt();
         this.defaultIndex = this.defaultIndex >>> 12 | this.defaultIndex << 20;
         this.timer = MenuItemTimerCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.timer)
         {
            this.timer.read(param1);
         }
         this.actionStyle = param1.readShort();
         this.showTooltipCooldownTimer = param1.readBoolean();
         this.status = MenuItemStatusCommand(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.status)
         {
            this.status.read(param1);
         }
         this.cooldownGroup = CooldownTypeModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.cooldownGroup)
         {
            this.cooldownGroup.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(9903);
         param1.writeShort(this.counterType);
         param1.writeInt(this.defaultIndex << 12 | this.defaultIndex >>> 20);
         if(null != this.timer)
         {
            this.timer.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeShort(this.actionStyle);
         param1.writeBoolean(this.showTooltipCooldownTimer);
         if(null != this.status)
         {
            this.status.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.cooldownGroup)
         {
            this.cooldownGroup.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

