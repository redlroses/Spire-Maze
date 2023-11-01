﻿namespace CodeBase.Services.Randomizer
{
  public interface IRandomService : IService
  {
    int Range(int minValue, int maxValue);
    float Range(float minValue, float maxValue);
  }
}