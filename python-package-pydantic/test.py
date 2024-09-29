import model as tapir_types
from type_converters import convert_ac_to_tapir, convert_tapir_to_ac


property_id_array_item ={"propertyId": {
                    "guid": "4A0D773E-33FB-4CF7-98FD-561044BCB21D"
                }}

pydantic_object = tapir_types.PropertyIdArrayItem(**property_id_array_item)
print(type(pydantic_object))

archicad_object = convert_tapir_to_ac(pydantic_object)
print(type(archicad_object))

pydantic_object = convert_ac_to_tapir(archicad_object)
print(type(pydantic_object))


