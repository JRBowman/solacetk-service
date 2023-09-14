using SolaceTK.Models;
using SolaceTK.Models.Artifacts;
using SolaceTK.Models.Behavior;
using SolaceTK.Models.Core;
using SolaceTK.Models.Environment;
using SolaceTK.Models.Events;
using SolaceTK.Models.Sound;
using SolaceTK.Models.WorkItems;
using System.Collections;
using System.Numerics;

namespace SolaceTK.Data
{
    /// <summary>
    /// Global Data Extensions for all Entities. This may get abstracted into a reusable extensions system
    /// at some point in the full release of Version 1 that will implement an interface and target each Entity Type
    /// with a set of common Extension Methods defined.
    /// </summary>
    public static class SolTkDataExtensions
    {

        #region Behaviors:

        public static SolTkSystem? Merge(this SolTkSystem entity, SolTkSystem model)
        {
            if (model == null) return entity;
            entity ??= new SolTkSystem();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;

            entity.Behaviors = entity.Behaviors.Merge(model.Behaviors);
            entity.VarData = entity.VarData.Merge(model.VarData);
            entity.Events = entity.Events.Merge(model.Events);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<SolTkSystem> Merge(this ICollection<SolTkSystem> entity, ICollection<SolTkSystem> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkSystem>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }

            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static SolTkState Merge(this SolTkState entity, SolTkState model)
        {
            if (model == null) return entity;
            entity ??= new SolTkState();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.Animation?.Merge(model.Animation);
            entity.Conditions = entity.Conditions.Merge(model.Conditions);
            entity.EndDelay = model.EndDelay;
            entity.StartDelay = model.StartDelay;
            entity.Interruptable = model.Interruptable;
            entity.NoOp = model.NoOp;

            entity.ParentId = model.ParentId;
            entity.RunCount = model.RunCount;
            entity.StateType = model.StateType;
            entity.BehaviorSystemId = model.BehaviorSystemId;
            entity.Events = entity.Events.Merge(model.Events);
            entity.NextStates = entity.NextStates.Merge(model.NextStates);
            entity.StartData?.Merge(model.StartData);
            entity.ActData?.Merge(model.ActData);
            entity.EndData?.Merge(model.EndData);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static Animation Merge(this Animation entity, Animation model)
        {
            if (model == null) return entity;
            entity ??= new Animation();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;

            entity.ActFrameData.Merge(model.ActFrameData);
            entity.Components = entity.Components.Merge(model.Components);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static AnimationData? Merge(this AnimationData? entity, AnimationData? model)
        {
            if (model == null) return entity;
            entity ??= new AnimationData();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.Enabled = model.Enabled;
            entity.Invert = model.Invert;
            entity.Loop = model.Loop;
            entity.Mirror = model.Mirror;
            entity.RunCount = model.RunCount;
            entity.Speed = model.Speed;

            entity.Frames = entity.Frames.Merge(model.Frames);

            return entity;
        }

        public static AnimationFrame Merge(this AnimationFrame entity, AnimationFrame model)
        {
            if (model == null) return entity;
            entity ??= new AnimationFrame();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Duration = model.Duration;
            entity.FrameData = model.FrameData;
            entity.Height = model.Height;
            entity.Width = model.Width;
            entity.Order = model.Order;
            entity.Speed = model.Speed;
            entity.Tags = model.Tags;
            entity.DownstreamData = entity.DownstreamData.Merge(model.DownstreamData);

            return entity;
        }

        public static ICollection<SolTkState> Merge(this ICollection<SolTkState> entity, ICollection<SolTkState> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkState>();
            //var removed = new List<int>();

            if (temp.Count != model.Count)
            {
                var addedBehaviors = model.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedBehaviors);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) temp[i].BehaviorSystemId = 0;
                else temp[i].Merge(m);
                //else _context.Entry(temp[i]).State = EntityState.Modified;
            }

            return temp;
        }

        public static ICollection<AnimationFrame> Merge(this ICollection<AnimationFrame> entity, ICollection<AnimationFrame> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<AnimationFrame>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Order = i;
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }

            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        #endregion

        #region Events:

        public static SolTkEvent Merge(this SolTkEvent entity, SolTkEvent model)
        {
            if (model == null) return entity;
            entity ??= new SolTkEvent();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.IsPhysicsEvent = model.IsPhysicsEvent;
            entity.DebounceTime = model.DebounceTime;
            entity.Tags = model.Tags;

            entity.Messages = entity.Messages.Merge(model.Messages);
            entity.DownstreamData = entity.DownstreamData.Merge(model.DownstreamData);
            entity.Conditions = entity.Conditions.Merge(model.Conditions);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<SolTkEvent> Merge(this ICollection<SolTkEvent> entity, ICollection<SolTkEvent> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkEvent>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedEvents = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedEvents);
            }

            // Existing:
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
                //else _context.Events.Add(temp[i]);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static SolTkMessage Merge(this SolTkMessage entity, SolTkMessage model)
        {
            if (model == null) return entity;
            entity ??= new SolTkMessage();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.TargetName = model.TargetName;
            entity.TargetType = model.TargetType;
            entity.Data = entity.Data.Merge(model.Data);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<SolTkMessage> Merge(this ICollection<SolTkMessage> entity, ICollection<SolTkMessage> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkMessage>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedEvents = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedEvents);
            }

            for (var i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        #endregion

        #region Core:

        public static ResourceCollection Merge(this ResourceCollection entity, ResourceCollection model)
        {
            if (model == null) return entity;

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.NPCs = entity.NPCs?.Merge(model.NPCs);
            entity.Behaviors = entity.Behaviors?.Merge(model.Behaviors);
            entity.Characters = entity.Characters?.Merge(model.Characters);
            entity.Controllers = entity.Controllers?.Merge(model.Controllers);
            entity.Navigation = entity.Navigation?.Merge(model.Navigation);
            entity.Objects = entity.Objects?.Merge(model.Objects);
            entity.Transports = entity.Transports?.Merge(model.Transports);
            //entity.SoundSets.Merge(model.SoundSets);
            //entity.SoundSources.Merge(model.SoundSources);
            //entity.TileSets.Merge(model.TileSets);
            //entity.Dialogs.Merge(model.Dialogs);
            //entity.Huds.Merge(model.Huds);
            //entity.Maps.Merge(model.Maps);
            //entity.Menus.Merge(model.Menus);

            entity.Updated = DateTime.UtcNow;

            return entity;
        }

        public static SolTkComponent Merge(this SolTkComponent entity, SolTkComponent model)
        {
            if (model == null) return entity;
            entity ??= new SolTkComponent();

            entity.Name = model.Name;
            entity.Description = model.Description;
            //entity.BehaviorAnimationId = model.BehaviorAnimationId;
            //entity.ControllerId = model.ControllerId;
            entity.PositionX = model.PositionX;
            entity.PositionY = model.PositionY;
            entity.PositionZ = model.PositionZ;
            entity.ScaleX = model.ScaleX;
            entity.ScaleY = model.ScaleY;
            entity.ScaleZ = model.ScaleZ;
            entity.RotationX = model.RotationX;
            entity.RotationY = model.RotationY;
            entity.RotationZ = model.RotationZ;
            entity.ComponentType = model.ComponentType;
            entity.ColorKey = model.ColorKey;
            entity.Tags = model.Tags;
            entity.Enabled = model.Enabled;
            entity.ComponentData = entity.ComponentData.Merge(model.ComponentData);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<SolTkComponent> Merge(this ICollection<SolTkComponent> entity, ICollection<SolTkComponent> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkComponent>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static SolTkData Merge(this SolTkData entity, SolTkData model)
        {
            if (model == null) return entity;
            entity ??= new SolTkData();

            entity.Key = model.Key;
            entity.Data = model.Data;
            entity.Operator = model.Operator;

            return entity;
        }

        public static ICollection<SolTkData> Merge(this ICollection<SolTkData> entity, ICollection<SolTkData> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkData>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static SolTkCondition Merge(this SolTkCondition entity, SolTkCondition model)
        {
            if (model == null) return entity;
            entity ??= new SolTkCondition();

            entity.Key = model.Key;
            entity.Data = model.Data;
            entity.Operator = model.Operator;

            return entity;
        }

        public static ICollection<SolTkCondition> Merge(this ICollection<SolTkCondition> entity, ICollection<SolTkCondition> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkCondition>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static SolTkArtifact Merge(this SolTkArtifact entity, SolTkArtifact model)
        {
            if (model == null) return entity;
            entity ??= new SolTkArtifact();

            return entity;
        }

        public static ICollection<SolTkArtifact> Merge(this ICollection<SolTkArtifact> entity, ICollection<SolTkArtifact> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkArtifact>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        #endregion

        #region Controllers:

        public static SolTkController Merge(this SolTkController entity, SolTkController model)
        {
            if (model == null) return entity;
            entity ??= new SolTkController();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.AffectedByGravity = model.AffectedByGravity;
            entity.BehaviorSystemId = model.BehaviorSystemId;
            entity.BodyType = model.BodyType;
            entity.CanMove = model.CanMove;
            entity.CollisionType = model.CollisionType;
            entity.MapPositionX = model.MapPositionX;
            entity.MapPositionY = model.MapPositionY;
            entity.Mass = model.Mass;
            entity.PixelKeyColor = model.PixelKeyColor;
            entity.Type = model.Type;
            entity.UseFriction = model.UseFriction;
            entity.WorldPositionX = model.WorldPositionX;
            entity.WorldPositionY = model.WorldPositionY;
            entity.WorldPositionZ = model.WorldPositionZ;

            entity.Components = entity.Components.Merge(model.Components);
            entity.ControllerData = entity.ControllerData.Merge(model.ControllerData);
            entity.SoundSet?.Merge(model.SoundSet);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<SolTkController> Merge(this ICollection<SolTkController> entity, ICollection<SolTkController> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SolTkController>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        #endregion

        #region Sounds:

        public static SoundSet Merge(this SoundSet entity, SoundSet model)
        {
            if (model == null) return entity;
            entity ??= new SoundSet();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static SoundSource Merge(this SoundSource entity, SoundSource model)
        {
            if (model == null) return entity;
            entity ??= new SoundSource();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;

            entity.SoundData = entity.SoundData.Merge(model.SoundData);
            //entity.Artifact.Merge(model.Artifact);
            entity.ArtifactId = model.ArtifactId;
            entity.PlayOnLoad = model.PlayOnLoad;
            entity.Volume = model.Volume;
            entity.ClipName = model.ClipName;
            entity.Channel = model.Channel;
            entity.IsLoop = model.IsLoop;
            entity.IsReactive = model.IsReactive;
            entity.Conditions = entity.Conditions.Merge(model.Conditions);
            entity.LoopEndTime = model.LoopEndTime;
            entity.LoopStartTime = model.LoopStartTime; 
            entity.Pitch = model.Pitch;

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<SoundSource> Merge(this ICollection<SoundSource> entity, ICollection<SoundSource> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<SoundSource>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        #endregion

        #region Projects:

        public static WorkProject Merge(this WorkProject entity, WorkProject model)
        {
            if (model == null) return entity;
            entity ??= new WorkProject();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;

            entity.WorkItems = entity.WorkItems.Merge(model.WorkItems);
            entity.IsProjectBillable = model.IsProjectBillable;
            entity.Comments = entity.Comments.Merge(model.Comments);
            entity.PaidHours = model.PaidHours;
            entity.TotalPaid = model.TotalPaid;
            entity.Payments = entity.Payments.Merge(model.Payments);

            if (entity.Payments != null && entity.Payments.Count > 0)
            {
                var totalAmountPaid = 0f;
                foreach (var payment in entity.Payments)
                {
                    totalAmountPaid += payment.Amount;
                }
                entity.TotalPaid = totalAmountPaid;
                entity.PaidHours = totalAmountPaid / 35f;
                entity.RemainingPayment = entity.TotalActualHours - entity.PaidHours;
            }

            if (entity.WorkItems != null && entity.WorkItems.Count > 0)
            {
                var totalHoursEstimate = 0f;
                var totalHoursActual = 0f;

                foreach (var item in entity.WorkItems)
                {
                    totalHoursEstimate += item.HoursEstimate;
                    totalHoursActual += item.HoursActual;
                }
                entity.TotalEstimatedHours = totalHoursEstimate;
                entity.TotalActualHours = totalHoursActual;
            }

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<WorkProject> Merge(this ICollection<WorkProject> entity, ICollection<WorkProject> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<WorkProject>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static WorkItem Merge(this WorkItem entity, WorkItem model)
        {
            if (model == null) return entity;
            entity ??= new WorkItem();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.HoursActual = model.HoursActual;
            entity.HoursEstimate = model.HoursEstimate;

            entity.Comments = entity.Comments.Merge(model.Comments);
            entity.Payment?.Merge(model.Payment);
            entity.IsPaid = entity.Payment != null && entity.Payment.Id > 0;
            entity.WorkProjectId = model.WorkProjectId;

            entity.Updated = DateTimeOffset.UtcNow;
            //entity.Artifacts.Merge(model.Artifacts);

            return entity;
        }

        public static ICollection<WorkItem> Merge(this ICollection<WorkItem> entity, ICollection<WorkItem> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !entity.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < entity.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static WorkComment Merge(this WorkComment entity, WorkComment model)
        {
            if (model == null) return entity;
            entity ??= new WorkComment();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.Comment = model.Comment;
            entity.Artifacts = entity.Artifacts.Merge(model.Artifacts);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<WorkComment> Merge(this ICollection<WorkComment> entity, ICollection<WorkComment> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<WorkComment>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static WorkPayment Merge(this WorkPayment entity, WorkPayment model)
        {
            if (model == null) return entity;
            entity ??= new WorkPayment();


            return entity;
        }

        public static ICollection<WorkPayment> Merge(this ICollection<WorkPayment> entity, ICollection<WorkPayment> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<WorkPayment>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        #endregion

        #region Environment:

        public static Map Merge(this Map entity, Map model)
        {
            if (model == null) return entity;
            entity ??= new Map();


            return entity;
        }

        public static ICollection<Map> Merge(this ICollection<Map> entity, ICollection<Map> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<Map>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static TileSet Merge(this TileSet entity, TileSet model)
        {
            if (model == null) return entity;
            entity ??= new TileSet();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;
            entity.Tiles = entity.Tiles.Merge(model.Tiles);

            entity.Updated = DateTimeOffset.UtcNow;

            return entity;
        }

        public static ICollection<TileSet> Merge(this ICollection<TileSet> entity, ICollection<TileSet> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<TileSet>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static Tile Merge(this Tile entity, Tile model)
        {
            if (model == null) return entity;
            entity ??= new Tile();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;

            entity.LX = model.LX;
            entity.LY = model.LY;
            entity.Width = model.Width;
            entity.Height = model.Height;
            entity.ColorKey = model.ColorKey;
            entity.Mode = model.Mode;
            entity.ObjectKey = model.ObjectKey;

            entity.Data = entity.Data.Merge(model.Data);
            entity.Rules = entity.Rules.Merge(model.Rules);

            entity.Updated = DateTimeOffset.UtcNow;


            return entity;
        }

        public static ICollection<Tile> Merge(this ICollection<Tile> entity, ICollection<Tile> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<Tile>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }

        public static TileRule Merge(this TileRule entity, TileRule model)
        {
            if (model == null) return entity;
            entity ??= new TileRule();

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Tags = model.Tags;

            entity.ColorKey = model.ColorKey;
            entity.VX = model.VX;
            entity.VY = model.VY;
            entity.CheckType = model.CheckType;
            entity.DataKey = model.DataKey;
            entity.Priority = model.Priority;
            entity.VM = model.VM;

            return entity;
        }

        public static ICollection<TileRule> Merge(this ICollection<TileRule> entity, ICollection<TileRule> model)
        {
            if (model == null) return entity;

            var temp = entity.ToList() ?? new List<TileRule>();
            var removed = new List<int>();

            // Merge Lists:
            if (temp.Count != model.Count)
            {
                var addedData = model.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id));
                temp.AddRange(addedData);
            }

            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Id == 0) continue;

                var m = model.FirstOrDefault(e => e.Id == temp[i].Id);
                if (m == null) removed.Add(temp[i].Id);
                else temp[i].Merge(m);
            }
            if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
            return temp;
        }


        #endregion

        // TODO: Figure out the best path to implement these generic / asbtracted update functions:
        //public static IModelTK Merge(this IModelTK entity, IModelTK model, Action<IModelTK, IModelTK> dataMerge)
        //{
        //    if (model == null) return entity;
        //    entity ??= default(IModelTK);

        //    entity.Name = model.Name;
        //    entity.Description = model.Description;
        //    entity.Tags = model.Tags;

        //    dataMerge.Invoke(entity, model);

        //    return entity;
        //}

        //public static ICollection<IModelTK> Merge(this ICollection<IModelTK> entity, ICollection<IModelTK> model)
        //{
        //    if (model == null) return entity;

        //    var temp = entity.ToList() ?? new List<IModelTK>();
        //    var current = model ?? new List<IModelTK>();
        //    var removed = new List<int>();

        //    // Merge Lists:
        //    if (temp.Count != model.Count)
        //    {
        //        var addedData = current.Where(x => x.Id == 0 || !temp.Any(t => t.Id == x.Id)) as ICollection<IModelTK>;
        //        if (addedData != null && addedData.Count > 0) temp.AddRange(addedData);
        //    }

        //    for (int i = 0; i < temp.Count; i++)
        //    {
        //        if (temp[i].Id == 0) continue;

        //        var m = current.FirstOrDefault(e => e.Id == temp[i].Id);
        //        if (m == null) removed.Add(temp[i].Id);
        //        else temp[i].Merge(m);
        //    }
        //    if (removed.Count > 0) temp.RemoveAll(s => removed.Contains(s.Id));
        //    return temp;
        //}

    }
}
