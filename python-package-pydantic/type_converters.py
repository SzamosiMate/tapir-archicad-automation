import model as tapir_types
import archicad.releases.ac27.b3001types as ac_types
from archicad.acbasetype import _get_constructor

def convert_ac_to_tapir(ac_type_instance: ac_types._ACBaseType) -> tapir_types.BaseModel:
    class_name = type(ac_type_instance).__name__
    try:
        tapir_class = getattr(tapir_types, class_name)
    except AttributeError:
        raise AttributeError(f"Type {class_name} doesn't exist in tapir_types")

    return tapir_class(**ac_type_instance.to_dict())


def convert_tapir_to_ac(tapir_type_instance: tapir_types.BaseModel) -> ac_types._ACBaseType:
    class_name = type(tapir_type_instance).__name__
    try:
        ac_class = getattr(ac_types, class_name)
    except AttributeError:
        raise AttributeError(f"Type {class_name} doesn't exist in ac_types")

    ac_class_constructor = _get_constructor(ac_class)
    return ac_class_constructor(tapir_type_instance.model_dump())



