import requests
import json
import time
from PIL import Image
import logging


def generate_prompt(description, style="a photograph of"):
    """Generate prompt to pass to diffusion model. Add style for consistent images.

    Args:
        description (str): Manual description of image.
        style (str, optional): Style prompt to add. See https://beta.dreamstudio.ai/prompt-guide for suggestions. Defaults to "a photograph of".

    Returns:
        _type_: _description_
    """
    return style + " " + description


def generate_image(
    prompt,
    api_key,
    model_version,
    pred_endpoint="https://api.replicate.com/v1/predictions",
    num_diffusion_steps=50,
):
    """Generate image using stable diffusion model.

    Args:
        prompt (string): Prompt to pass to stable diffusion model api.
        api_key(str): API key for replicate.com.
        model_version (str): Key of stable diffusion model to use.
        pred_endpoint (str, optional): Base endpoint for replicate api. Defaults to "https://api.replicate.com/v1/predictions".
        num_diffusion_steps (int, optional): Number of inference diffusion steps to run. Higher gives better quality output for larger inference time. Defaults to 50.

    Returns:
        output_image_url (str): URL of generated image.
        generation_time (float): Time to generate image.
    """
    start = time.time()
    json_input = {
        "version": model_version,
        "input": {"prompt": prompt, "num_inference_steps": num_diffusion_steps},
    }
    response = requests.post(
        pred_endpoint, json=json_input, headers={"Authorization": f"Token {api_key}"}
    )
    json_response = json.loads(response.text)
    get_url = json_response["urls"]["get"]

    while True:
        response = requests.get(
            get_url, headers={"Authorization": f"Token {api_key}"}, stream=True
        )
        content = json.loads(response.content)
        status = content["status"]
        if status == "succeeded":
            break
        time.sleep(1)

    output_image_url = content["output"][0]
    end = time.time()
    generation_time = end - start
    logging.debug(f"Time to generate image: {generation_time}")

    return output_image_url, generation_time


def display_image_from_url(output_image_url):
    """Display image from url

    Args:
        output_image_url (str): URL of image to display.
    """
    im = Image.open(requests.get(output_image_url, stream=True).raw)
    im.show()
