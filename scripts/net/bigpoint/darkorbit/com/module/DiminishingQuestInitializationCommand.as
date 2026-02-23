package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class DiminishingQuestInitializationCommand implements IModule
   {
      private static var _instance:DiminishingQuestInitializationCommand;
      
      public var timeToQuestEnd:Number = 0;
      
      public var acceptabilityStatus:QuestAcceptabilityStatusModule;
      
      public var quest:QuestDefinitionModule;
      
      public function DiminishingQuestInitializationCommand(param1:QuestDefinitionModule = null, param2:QuestAcceptabilityStatusModule = null, param3:Number = 0)
      {
         super();
         this.timeToQuestEnd = param3;
         if(param2 == null)
         {
            this.acceptabilityStatus = new QuestAcceptabilityStatusModule();
         }
         else
         {
            this.acceptabilityStatus = param2;
         }
         if(param1 == null)
         {
            this.quest = new QuestDefinitionModule();
         }
         else
         {
            this.quest = param1;
         }
      }
      
      public static function get instance() : DiminishingQuestInitializationCommand
      {
         return _instance || (_instance = new DiminishingQuestInitializationCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 602;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 8;
      }
      
      public function read(param1:IDataInput) : void
      {
         this.timeToQuestEnd = param1.readDouble();
         this.acceptabilityStatus = QuestAcceptabilityStatusModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.acceptabilityStatus)
         {
            this.acceptabilityStatus.read(param1);
         }
         this.quest = QuestDefinitionModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.quest)
         {
            this.quest.read(param1);
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         param1.writeShort(602);
         param1.writeDouble(this.timeToQuestEnd);
         if(null != this.acceptabilityStatus)
         {
            this.acceptabilityStatus.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.quest)
         {
            this.quest.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
      }
   }
}

