using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using GameData;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace DataPipeline
{
    [ContentProcessor(DisplayName = "DataPipeline.TilemapProcessor")]
    public class TilemapProcessor : ContentProcessor<PipelineTilemapContent, PipelineTilemapContent>
    {
        public override PipelineTilemapContent Process(PipelineTilemapContent input, ContentProcessorContext context)
        {
            foreach (string property in input.Properties.Keys)
            {
                switch (property)
                {
                    case "Music":
                        input.tmMusic = input.Properties[property];
                        break;

                    default:
                        ParamArrayAttribute[] attrs = new ParamArrayAttribute[0];
                        context.Logger.LogMessage("Unknown property: " + property, attrs);
                        break;
                }
            }

            for (int i = 0; i < input.tmImagePaths.Length; i++)
            {
                string filename = Path.GetFileNameWithoutExtension(input.tmImagePaths[i]);
                string[] directory = Path.GetDirectoryName(input.tmImagePaths[i]).Split(Path.DirectorySeparatorChar);
                int directoriesToInclude = 3;
                string fullDirectory = "";
                for (int j = directory.Length - directoriesToInclude; j < directory.Length; j++)
                {
                    fullDirectory += (directory[j] + Path.DirectorySeparatorChar);
                }
                input.tmImagePaths[i] = Path.Combine(fullDirectory, filename);
            }

            for (int i = 0; i < input.tmLayerCount; i++)
            {
                foreach (string property in input.tmLayers[i].Properties.Keys)
                {
                    switch (property)
                    {
                        case "ScrollingSpeed":
                            input.tmLayers[i].ScrollSpeed = float.Parse(input.tmLayers[i].Properties["ScrollingSpeed"]);
                            break;

                        case "ScrollOffset":
                            input.tmLayers[i].ScrollOffset = float.Parse(input.tmLayers[i].Properties["ScrollOffset"]);
                            break;

                        case "LayerDepth":
                            input.tmLayers[i].LayerDepth = float.Parse(input.tmLayers[i].Properties["LayerDepth"]);
                            break;

                        default:
                            ParamArrayAttribute[] attrs = new ParamArrayAttribute[0];
                            context.Logger.LogMessage("Unknown property: " + property, attrs);
                            break;
                    }
                }
            }

            for (int i = 0; i < input.tmGameEntityGroupCount; i++)
            {
                foreach (string property in input.tmGameEntityGroups[i].Properties.Keys)
                {
                    switch (property)
                    {
                        case "ScrollingSpeed":
                            input.tmGameEntityGroups[i].ScrollSpeed = float.Parse(input.tmGameEntityGroups[i].Properties["ScrollingSpeed"]);
                            break;
                        case "ScrollOffset":
                            input.tmGameEntityGroups[i].ScrollOffset = float.Parse(input.tmGameEntityGroups[i].Properties["ScrollOffset"]);
                            break;
                        case "LayerDepth":
                            input.tmGameEntityGroups[i].LayerDepth = float.Parse(input.tmGameEntityGroups[i].Properties["LayerDepth"]);
                            break;
                        default:
                            ParamArrayAttribute[] attrs = new ParamArrayAttribute[0];
                            context.Logger.LogMessage("Unknown property: " + property, attrs);
                            break;
                    }

                    foreach (GameEntity goData in input.tmGameEntityGroups[i].GameEntityData)
                    {
                        if (goData.entCategory == "PlayerStart")
                        {
                            input.tmPlayerStart = new Vector2(goData.entPosition.Center.X, goData.entPosition.Center.Y);
                            input.tmPlayerLayer = i;
                        }
                    }
                }
            }

            return input;
        }
    }
}
