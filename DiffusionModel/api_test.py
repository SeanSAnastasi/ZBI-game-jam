from api_wrappers import (
    generate_prompt,
    generate_image,
    display_image_from_url,
)

pred_endpoint = "https://api.replicate.com/v1/predictions"
api_key = "411758a7bd9e0c142c9c56a4a2fea3d7833c0195"
model_version = "a9758cbfbd5f3c2094457d996681af52552901775aa2d6dd0b17fd15df959bef"  # Stable diffusion
backup_model_version = (
    "9218d6e9a0f5f147f53f31ab6a2d562e5e88a3dab5c2f74fce9e067b868b9ca4"  # Glide
)

style = "an oil painting"
description = "a small cottage with a thatched roof and wooden exterior next to a lake with mountains in the background"
num_diffusion_steps = 50
respace = 250
display_image = True
print_duration = True


def main():
    prompt = generate_prompt(description=description, style=style)
    try:
        output_image_url, generation_time = generate_image(
            prompt=prompt,
            api_key=api_key,
            model_version=model_version,
            pred_endpoint=pred_endpoint,
            num_diffusion_steps=num_diffusion_steps,
        )
    except:
        output_image_url, generation_time = generate_image(
            prompt=prompt,
            api_key=api_key,
            model_version=backup_model_version,
            pred_endpoint=pred_endpoint,
            respace=respace,
        )
    if display_image:
        display_image_from_url(output_image_url)
    if print_duration:
        print(f"Generation_time: {generation_time}")
    return output_image_url


if __name__ == "__main__":
    main()
