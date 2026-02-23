package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class UIItemAddHighlightCommand implements IModule
   {
      private static var _instance:UIItemAddHighlightCommand;
      
      public var itemSource:UiItemSourceModule;
      
      public var soundModule:SoundTriggerModule;
      
      public var highlightType:HighlightTypeModule;
      
      public var itemId:String = "";
      
      public function UIItemAddHighlightCommand(param1:UiItemSourceModule = null, param2:String = "", param3:HighlightTypeModule = null, param4:SoundTriggerModule = null)
      {
         super();
         if(param1 == null)
         {
            this.itemSource = new UiItemSourceModule();
         }
         else
         {
            this.itemSource = param1;
         }
         if(param4 == null)
         {
            this.soundModule = new SoundTriggerModule();
         }
         else
         {
            this.soundModule = param4;
         }
         if(param3 == null)
         {
            this.highlightType = new HighlightTypeModule();
         }
         else
         {
            this.highlightType = param3;
         }
         this.itemId = param2;
      }
      
      public static function get instance() : UIItemAddHighlightCommand
      {
         return _instance || (_instance = new UIItemAddHighlightCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -16009;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 2;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.itemSource = UiItemSourceModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.itemSource)
         {
            this.itemSource.read(param1);
         }
         this.soundModule = SoundTriggerModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.soundModule)
         {
            this.soundModule.read(param1);
         }
         this.highlightType = HighlightTypeModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.highlightType)
         {
            this.highlightType.read(param1);
         }
         this.itemId = param1.readUTF();
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(-16009);
         if(null != this.itemSource)
         {
            this.itemSource.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.soundModule)
         {
            this.soundModule.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.highlightType)
         {
            this.highlightType.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeUTF(this.itemId);
      }
   }
}

