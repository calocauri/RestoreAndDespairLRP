public static class InteractionHandler {

    public static int CalculateInteractionResult(MaterialType materialType, InteractionType interactionType) {
        if (interactionType == InteractionType.Heal) {
            return 1;
        }

        switch (materialType) {
            case MaterialType.Crystal:
                return CrystalInteractionResult(interactionType);
            case MaterialType.Organic:
                return OrganicInteractionResult(interactionType);
            case MaterialType.Paper:
                return PaperInteractionResult(interactionType);
            case MaterialType.Stone:
                return StoneInteractionResult(interactionType);
            case MaterialType.Wood:
                return WoodInteractionResult(interactionType);
            default:
                return 1;
        }
    }

    private static int CrystalInteractionResult(InteractionType interactionType) {
        return -2;
    }

    private static int OrganicInteractionResult(InteractionType interactionType) {
        if (interactionType == InteractionType.Cut) {
            return -2;
        }
        return -1;
    }

    private static int PaperInteractionResult(InteractionType interactionType) {
        if (interactionType == InteractionType.Blunt) {
            return -1;
        }
        return -2;
    }

    private static int StoneInteractionResult(InteractionType interactionType) {
        if (interactionType == InteractionType.Blunt) {
            return -2;
        }
        return -1;
    }

    private static int WoodInteractionResult(InteractionType interactionType) {
        if (interactionType == InteractionType.Pierce) {
            return -1;
        }
        return -2;
    }

}